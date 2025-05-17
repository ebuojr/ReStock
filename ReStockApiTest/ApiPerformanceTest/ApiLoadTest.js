// Guidance: How to Run this k6 Load Test
// --------------------------------------------------
// 1. Install k6 if you haven't already:
//    - Download from https://k6.io/docs/getting-started/installation/
//    - Or use: choco install k6 (on Windows with Chocolatey)
//
// 2. Start your API server so it is accessible at the BASE_URL defined below.
//    - Default: https://localhost:7257/api
//    - Adjust BASE_URL if your API runs on a different port or host.
//
// 3. Open a terminal and navigate to this file's directory.
//
// 4. Run the test with:
//    k6 run ApiLoadTest.js
//
// 5. View the results in the terminal. 
//
// --------------------------------------------------

import http from 'k6/http';
import { group, sleep } from 'k6';

export let options = {
  stages: [
    { duration: '30s', target: 5 },   // Ramp up to 5 users
    { duration: '1m', target: 10 },    // Stay at 10 users
    { duration: '30s', target: 0 },    // Ramp down to 0 users
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