// k6 performance test for ReStock API
// Each group targets a specific API endpoint to measure response time and reliability
// Example parameters are used for endpoints that require them
// Use k6's summary and tags to analyze per-endpoint performance after running the test

import http from 'k6/http';
import { group, sleep } from 'k6';

export let options = {
  stages: [
    // { duration: '30s', target: 5 },   // Ramp up to 5 users
    { duration: '1m', target: 10 },    // Stay at 10 users
    // { duration: '30s', target: 0 },    // Ramp down to 0 users
  ],
};

const BASE_URL = 'https://localhost:7257/api'; // Adjust if your API runs on a different port

export default function () {
  // Get all products
  group('Get All Products', function () {
    http.get(`${BASE_URL}/Product/all`);
  });

  // Get a single product by item number
  group('Get Product By No', function () {
    http.get(`${BASE_URL}/Product/get/ZIZ-302-5622`); // Example ItemNo
  });

  // Get all stores
  group('Get All Stores', function () {
    http.get(`${BASE_URL}/Store/all`);
  });

  // Get a single store by store number
  group('Get Store By No', function () {
    http.get(`${BASE_URL}/Store/get/5002`); // Example StoreNo
  });

  // Get store inventory with thresholds for a specific store
  group('Get Store Inventory With Thresholds', function () {
    http.get(`${BASE_URL}/inventory/store-inventory-with-Threshold-store-no?storeNo=5002`); // Example storeNo
  });

  // Get store inventory by store number
  group('Get Store Inventory By Store No', function () {
    http.get(`${BASE_URL}/inventory/store?storeNo=5002`); // Example storeNo
  });

  // Get store inventory by store and item number
  group('Get Store Inventory By Store and Item', function () {
    http.get(`${BASE_URL}/inventory/store-item?storeNo=5002&ItemNo=ZIZ-302-5622`); // Example storeNo and ItemNo
  });

  // Get distribution center inventory
  group('Get Distribution Center Inventory', function () {
    http.get(`${BASE_URL}/inventory/distribution-center-inventory`);
  });

  sleep(1); // Wait between iterations
}