Feature: Calculator

Simple calculator for adding two numbers

@mytag
Scenario Outline: Add two numbers
    Given the first number is <FirstNumber>
    And the second number is <SecondNumber>
    When the two numbers are added
    Then the result should be <Result>

    Examples:
      | FirstNumber | SecondNumber | Result |
      | 50          | 70           | 120    |
      | 20          | 30           | 50     |