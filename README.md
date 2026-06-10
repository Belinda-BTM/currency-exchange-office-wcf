# Currency Exchange Office



**Course:** Network Application Development

**Project title:** Currency Exchange Office — WCF Service with WPF Client

**Author:** Belinda Tanatswa Mandudzo

**Student ID:** 64290



## Project Description



A distributed currency exchange office system built with .NET, covering Labs 1–14 of the Network Application Development course. The system consists of:



- **WCF Service** — a Windows Communication Foundation web service (C#) exposing currency exchange operations: retrieving available currencies and calculating exchanges between currencies.

- **WPF Client** — a desktop client application (ExchangeOffice.WpfClient) that consumes the WCF service, with an interface for performing conversions and viewing all available currencies. A console client is also included.

- **Database** — SQL Server LocalDB (ExchangeOfficeDB) with **Users** and **Transactions** tables for storing user records and transaction history.



## Repository Structure



```

/currency-exchange-office-wcf

│

├── CurrencyExchangeOffice/   - Visual Studio solution: WCF service projects

│                             - and client applications 

├── Database/                 - SQL scripts (schema.sql)

├── Documentation/            - Project report

└── README.md

```



## How to Run the Project



### Prerequisites

- Windows 10/11

- Visual Studio 2022 with the **.NET desktop development**

- SQL Server Express LocalDB (installed with Visual Studio)



### Steps

1. Clone the repository: git clone https://github.com/Belinda-BTM/currency-exchange-office-wcf.git

2. Open the solution file (.sln) inside the CurrencyExchangeOffice folder in Visual Studio 2022.

3. Set up the database: execute Database/schema.sql against your LocalDB instance (localdb)\\MSSQLLocalDB (e.g. via View → SQL Server Object Explorer → New Query).

4. Restore NuGet packages: right-click the solution → **Restore NuGet Packages**.

5. Start the WCF service project first (right-click → Set as Startup Project → F5).

6. With the service running, start the WPF client: right-click ExchangeOffice.WpfClient → **Debug → Start New Instance**.

7. Use the client interface to perform currency exchange operations.



### Notes

- If the client cannot reach the service, check that the endpoint address in the client's App.config matches the running service address.

- The database connection string is in the service project's configuration file and points to `(localdb)\\MSSQLLocalDB`.

