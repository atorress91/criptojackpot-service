import http from 'k6/http';
import { check } from 'k6';
import exec from 'k6/execution';

export const options = {
  vus: 200,
  duration: '10s',
};

const BASE_URL = 'http://localhost:5500';

export default function main() {
  const uniqueId = `${exec.vu.idInTest}-${exec.scenario.iterationInTest}-${Date.now()}`;
  const uniqueEmail = `loadtest-${uniqueId}@example.com`;
  const uniqueIdentification = `ID-${uniqueId}`;

  const payload = JSON.stringify({
    name: 'Load',
    lastName: 'Tester',
    email: uniqueEmail, // <--- ÚNICO: Evita bloqueo de fila en BD
    password: 'Test123$',
    identification: uniqueIdentification, // <--- ÚNICO: Evita bloqueo por ID
    phone: '88888888',
    roleId: 2,
    countryId: 54,
    statePlace: 'San Jose',
    city: 'San Jose',
    address: 'Av 10',
    status: true,
    imagePath: 'string',
    googleAccessToken: 'string',
    googleRefreshToken: 'string',
  });

  const params = {
    headers: {
      'Content-Type': 'application/json',
      // Authorization: `Bearer ${token}`,
    },
  };

  const res = http.post(`${BASE_URL}/api/v1/User`, payload, params);

  // 201 = Created, 409 = Conflict (duplicado), 503 = Service Unavailable (pool agotado)
  if (res.status !== 201 && res.status !== 409 && res.status !== 503) {
    console.log(`Status inesperado: ${res.status}, body: ${res.body}`);
  }

  check(res, {
    'status es 201, 409 o 503': r => r.status === 201 || r.status === 409 || r.status === 503,
  });
}
