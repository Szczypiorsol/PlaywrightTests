Feature: Sprzedaż produktów w sklepie Swag Labs (saucedemo.com)

  Scenario: Złożenie zamówienia dla wybranego produktu z poprawnymi danymi klienta
    Given Użytkownik jest na stronie logowania Swag Labs
    When Loguje się jako "standard_user" z hasłem "secret_sauce"
    And Dodaje produkt "Sauce Labs Backpack" do koszyka
    And Przechodzi do koszyka
    And Rozpoczyna realizację zamówienia
    And Podaje dane klienta: imię "Jan", nazwisko "Kowalski", kod "12345"
    And Przechodzi do podsumowania zamówienia
    And Finalizuje zamówienie
    Then Powinien zobaczyć potwierdzenie zamówienia "Thank you for your order!"

  Scenario: Dodanie produktu do koszyka powoduje widoczność produktu w koszyku
    Given Użytkownik jest na stronie logowania Swag Labs
    When Loguje się jako "standard_user" z hasłem "secret_sauce"
    And Dodaje produkt "Sauce Labs Bike Light" do koszyka
    And Przechodzi do koszyka
    Then Koszyk powinien zawierać produkt "Sauce Labs Bike Light"

  Scenario: Usunięcie produktu z koszyka powoduje pusty koszyk
    Given Użytkownik jest na stronie logowania Swag Labs
    When Loguje się jako "standard_user" z hasłem "secret_sauce"
    And Dodaje produkt "Sauce Labs Bike Light" do koszyka
    And Przechodzi do koszyka
    And Usuwa produkt "Sauce Labs Bike Light" z koszyka
    Then Koszyk powinien być pusty

  Scenario Outline: Licznik koszyka odzwierciedla liczbę dodanych produktów 
    Given Użytkownik jest na stronie logowania Swag Labs
    When Loguje się jako "standard_user" z hasłem "secret_sauce"
    And Dodaje produkty do koszyka: <produkty>
    Then Licznik koszyka powinien wynosić <liczba>

    Examples:
      | produkty                                                | liczba |
      | "Sauce Labs Backpack"                                   | 1      |
      | "Sauce Labs Backpack", "Sauce Labs Bike Light"          | 2      |

  Scenario Outline: Logowanie odrzucone dla niepoprawnych lub brakujących danych
    Given Użytkownik jest na stronie logowania Swag Labs
    When Loguje się jako "<uzytkownik>" z hasłem "<haslo>"
    Then Powinien zobaczyć komunikat błędu "<komunikat>"

    Examples:
      | uzytkownik       | haslo           | komunikat                                                            |
      | standard_user    | wrong_password  | Epic sadface: Username and password do not match any user in this service |
      |                  | secret_sauce    | Epic sadface: Username is required                                   |
      | standard_user    |                 | Epic sadface: Password is required                                   |

  Scenario: Logowanie odrzucone dla zablokowanego użytkownika
    Given Użytkownik jest na stronie logowania Swag Labs
    When Loguje się jako "locked_out_user" z hasłem "secret_sauce"
    Then Powinien zobaczyć komunikat błędu "Epic sadface: Sorry, this user has been locked out."

  Scenario Outline: Walidacja wymaganych danych klienta podczas realizacji zamówienia 
    Given Użytkownik jest na stronie logowania Swag Labs
    When Loguje się jako "standard_user" z hasłem "secret_sauce"
    And Dodaje produkt "Sauce Labs Backpack" do koszyka
    And Przechodzi do koszyka
    And Rozpoczyna realizację zamówienia
    And Podaje dane klienta: imię "<imie>", nazwisko "<nazwisko>", kod "<kod>"
    And Próbuje przejść do podsumowania zamówienia
    Then Powinien zobaczyć komunikat walidacyjny "<komunikat>"

    Examples:
      | imie | nazwisko  | kod   | komunikat                      |
      |      | Kowalski  | 12345 | Error: First Name is required  |
      | Jan  |           | 12345 | Error: Last Name is required   |
      | Jan  | Kowalski  |       | Error: Postal Code is required |

  Scenario Outline: Sortowanie listy produktów zmienia kolejność wyświetlania 
    Given Użytkownik jest na stronie logowania Swag Labs
    When Loguje się jako "standard_user" z hasłem "secret_sauce"
    And Wybiera sortowanie "<sortowanie>"
    Then Produkty powinny być posortowane zgodnie z "<sortowanie>"

    Examples:
      | sortowanie             |
      | Name (A to Z)          |
      | Name (Z to A)          |
      | Price (low to high)    |
      | Price (high to low)    |