import http from 'k6/http';
import {check, sleep} from 'k6';
import {Rate, Trend, Counter} from 'k6/metrics';

// Métricas personalizadas
const loginSuccessRate = new Rate('login_success_rate');
const loginDuration = new Trend('login_duration');
const loginFailures = new Counter('login_failures');

export const options = {
    stages: [
        {duration: '30s', target: 50},   // Calentamiento: 50 usuarios
        {duration: '1m', target: 100},   // Incremento a 100 usuarios
        {duration: '1m', target: 200},   // Incremento a 200 usuarios
        {duration: '1m', target: 300},   // Incremento a 300 usuarios
        {duration: '1m', target: 500},   // Incremento a 500 usuarios
        {duration: '2m', target: 500},   // Mantener 500 usuarios
        {duration: '1m', target: 1000},  // Pico de estrés: 1000 usuarios
        {duration: '2m', target: 1000},  // Mantener 1000 usuarios
        {duration: '30s', target: 0},    // Desacelerar
    ],
    thresholds: {
        // El 95% de los requests deben completarse en menos de 2 segundos
        'http_req_duration': ['p(95)<2000'],
        // El 99% de los requests deben tener éxito
        'login_success_rate': ['rate>0.99'],
        // Menos del 1% de fallos permitidos
        'http_req_failed': ['rate<0.01'],
    },
};

const BASE_URL = 'http://localhost:5500';

// Lista de usuarios de prueba
// NOTA: Deberás tener estos usuarios creados en la BD o usar el mismo usuario
const testUsers = [
    {email: 'andreshts@yahoo.com', password: 'Test123$'}
];

export default function main() {
    // Seleccionar un usuario aleatorio de la lista
    const user = testUsers[Math.floor(Math.random() * testUsers.length)];

    const loginPayload = JSON.stringify({
        email: user.email,
        password: user.password,
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
            'Accept-Language': 'es',
        },
        tags: {name: 'Login'},
    };

    // Medir el tiempo de inicio
    const startTime = new Date();

    // Realizar el login
    const res = http.post(`${BASE_URL}/api/v1/auth`, loginPayload, params);

    // Medir la duración
    const duration = Date.now() - startTime;
    loginDuration.add(duration);

    // Verificar el resultado
    const success = check(res, {
        'status es 200': (r) => r.status === 200,
        'tiene token en la respuesta': (r) => {
            if (r.status === 200 && r.body) {
                const body = JSON.parse(r.body);
                return body?.data?.token !== undefined;
            }
            return false;
        },
        'response time < 2s': (r) => r.timings.duration < 2000,
        'response time < 1s': (r) => r.timings.duration < 1000,
        'response time < 500ms': (r) => r.timings.duration < 500,
    });

    // Registrar métricas
    loginSuccessRate.add(success);

    if (!success) {
        loginFailures.add(1);
        console.log(`Login falló para ${user.email}. Status: ${res.status}, Body: ${res.body}`);
    }

    // Simular tiempo de espera entre requests (pensar del usuario)
    sleep(Math.random() * 2 + 1); // Entre 1 y 3 segundos
}

export function handleSummary(data) {
    return {
        'login-load-test-summary.json': JSON.stringify(data, null, 2),
        stdout: textSummary(data, {indent: ' ', enableColors: true}),
    };
}

function textSummary(data, options = {}) {
    const indent = options.indent || '';
    const enableColors = options.enableColors || false;

    let summary = '\n' + indent + '========== RESUMEN DE PRUEBA DE LOGIN ==========\n\n';

    // Información general
    summary += indent + `Duración total: ${data.state.testRunDurationMs / 1000}s\n`;
    summary += indent + `VUs máximos: ${data.metrics.vus_max.values.max}\n\n`;

    // Requests HTTP
    const httpReqs = data.metrics.http_reqs.values;
    summary += indent + '--- Requests HTTP ---\n';
    summary += indent + `Total de requests: ${httpReqs.count}\n`;
    summary += indent + `Rate: ${httpReqs.rate.toFixed(2)} req/s\n\n`;

    // Duración de requests
    const httpDuration = data.metrics.http_req_duration.values;
    summary += indent + '--- Tiempos de respuesta ---\n';
    summary += indent + `Promedio: ${httpDuration.avg.toFixed(2)}ms\n`;
    summary += indent + `Mínimo: ${httpDuration.min.toFixed(2)}ms\n`;
    summary += indent + `Máximo: ${httpDuration.max.toFixed(2)}ms\n`;
    summary += indent + `Mediana: ${httpDuration.med.toFixed(2)}ms\n`;
    summary += indent + `P90: ${httpDuration['p(90)'].toFixed(2)}ms\n`;
    summary += indent + `P95: ${httpDuration['p(95)'].toFixed(2)}ms\n`;
    summary += indent + `P99: ${httpDuration['p(99)'].toFixed(2)}ms\n\n`;

    // Tasa de éxito
    if (data.metrics.login_success_rate) {
        const successRate = data.metrics.login_success_rate.values.rate;
        summary += indent + '--- Tasa de éxito ---\n';
        summary += indent + `Login success rate: ${(successRate * 100).toFixed(2)}%\n`;
    }

    // Fallos
    if (data.metrics.http_req_failed) {
        const failRate = data.metrics.http_req_failed.values.rate;
        summary += indent + `Request failure rate: ${(failRate * 100).toFixed(2)}%\n`;
    }

    if (data.metrics.login_failures) {
        summary += indent + `Total de fallos: ${data.metrics.login_failures.values.count}\n`;
    }

    summary += '\n' + indent + '===============================================\n';

    return summary;
}

