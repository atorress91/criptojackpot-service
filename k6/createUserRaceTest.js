import http from 'k6/http';
import { check } from 'k6';

export const options = {
  vus: 200,
  duration: '10s',
};

const BASE_URL = 'http://localhost:5500';
const TARGET_EMAIL = 'race-test@example.com';
const TARGET_ID = 'RACE-USER-001';

export default function main() {
  const payload = JSON.stringify({
    name: 'string',
    lastName: 'string',
    email: TARGET_EMAIL, // MISMO email para todos
    password: 'Test123$',
    identification: TARGET_ID, // MISMO identification para todos
    phone: 'string',
    roleId: 2,
    countryId: 54,
    statePlace: 'string',
    city: 'string',
    address: 'string',
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
