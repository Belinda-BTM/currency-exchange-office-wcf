CREATE TABLE [dbo].[Users] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [Username] NVARCHAR (50)   NOT NULL,
    [Password] NVARCHAR (50)   NOT NULL,
    [Balance]  DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC)
);

CREATE TABLE [dbo].[Transactions] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Username]     NVARCHAR (50)   NOT NULL,
    [Type]         NVARCHAR (10)   NOT NULL,
    [CurrencyCode] NVARCHAR (10)   NOT NULL,
    [Amount]       DECIMAL (18, 2) NOT NULL,
    [RateAtTime]   DECIMAL (18, 4) NOT NULL,
    [PlnValue]     DECIMAL (18, 2) NOT NULL,
    [Date]         DATETIME        DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

