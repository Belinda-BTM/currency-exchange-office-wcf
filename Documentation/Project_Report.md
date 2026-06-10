# Currency Exchange Office — Project Report

**Course:** Network Application Development
**Author:** Belinda Tanatswa Mandudzo
**Student ID:** 64290

## 1. Overview

This project implements a Currency Exchange Office system as a distributed .NET application, developed incrementally across the Network Application Development course. The system allows users to view available currencies, calculate currency conversions, and record exchange transactions.

## 2. Architecture

The solution consists of three main parts:

**WCF Service** — A Windows Communication Foundation web service written in C#. It exposes the core operations of the exchange office, including retrieving the list of available currencies (`GetAvailableCurrencies`) and performing exchange calculations between two currencies (`CalculateExchange`). The service handles invalid currency codes gracefully by returning an error result.

**WPF Client** — A desktop client application (`ExchangeOffice.WpfClient`) built with Windows Presentation Foundation. It consumes the WCF service through a generated service reference. The user can enter a source currency, a target currency, and an amount, then view the conversion result. An "All Currencies" button displays the full list of supported currencies. A console client (`Lab2.ConsoleClient`) from the earlier labs is also included to demonstrate basic service consumption.

**Database** — A SQL Server LocalDB database (`ExchangeOfficeDB`) containing two tables: **Users** and **Transactions**. The database stores user records and a history of performed exchange transactions. The schema script is provided in the `Database/schema.sql` file.

## 3. Technologies Used

- C# / .NET Framework
- Windows Communication Foundation (WCF)
- Windows Presentation Foundation (WPF)
- SQL Server Express LocalDB
- Visual Studio 2022
- Git / GitHub for version control

## 4. Development Process

The project was developed step by step following the course lab structure, with each lab committed separately to the GitHub repository. Commits document the progression including the addition of the All Currencies feature.

## 5. How to Run

Detailed run instructions are provided in the repository `README.md`. In short: open the solution in Visual Studio 2022, ensure the LocalDB database is available, start the WCF service, then start the WPF client.