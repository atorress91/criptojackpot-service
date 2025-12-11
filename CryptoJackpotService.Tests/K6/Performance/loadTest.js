import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '1m', target: 200 }, // calentamiento
    { duration: '2m', target: 400 }, // carga moderada
    { duration: '2m', target: 800 }, // carga fuerte
    { duration: '2m', target: 1200 }, // estrés
    { duration: '1m', target: 0 }, // desacelerar
  ],
};

const BASE_URL = 'http://localhost:5500';

export function setup() {
  const loginPayload = JSON.stringify({
    email: 'andreshts@yahoo.com',
    password: 'Test123$', // tu password
  });

  const params = {
    headers: {
      'Content-Type': 'application/json',
    },
  };

  const res = http.post(`${BASE_URL}/api/v1/auth`, loginPayload, params);

  // Log rápido si falla
  console.log('Login status:', res.status);

  check(res, {
    'login status 200': r => r.status === 200,
  });

  if (res.status !== 200 || !res.body) {
    console.error('Error en login. Respuesta:', res.body);
    throw new Error('Login falló, no se puede continuar con la prueba');
  }

  const body = res.json();

  const token = body?.data?.token;

  if (!token) {
    console.error('Body del login:', JSON.stringify(body));
    throw new Error('No se obtuvo el token del login');
  }

  return { token };
}

export default function main(data) {
  const token = data.token;

  const params = {
    headers: {
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    },
  };

  const res = http.get(`${BASE_URL}/api/v1/Prize?PageNumber=1&PageSize=10`, params);

  check(res, {
    'status 200': r => r.status === 200,
  });

  sleep(1);
}
