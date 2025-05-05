Feature: Create Potential Orders
  As a store manager
  I want to create potential orders for items below minimum quantity
  So that I can maintain optimal inventory levels

  Scenario: Create potential orders through API
    Given a store exists with store number 1
    And the store inventory is:
      | ItemNo  | CurrentQuantity | MinimumQuantity | TargetQuantity | ReorderQuantity |
      | ITEM001 | 5              | 10              | 20             | 10              |
    And the distribution center inventory is:
      | ItemNo  | Quantity |
      | ITEM001 | 50       |
    When I request potential orders for store 1
    Then the API should return:
      | StoreNo | ItemNo  | Quantity |
      | 1       | ITEM001 | 15       |