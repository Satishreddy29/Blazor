# Customer Order Tracking Application

## Overview

The Customer Order Tracking Application is a full-stack solution built using Blazor for the UI, along with multiple APIs to handle customer details, order management, tracking, and authentication. The application is designed to streamline customer interactions, provide order tracking functionalities, and ensure secure access through JWT authentication.

## Components

1. **Blazor UI Application**
   - A responsive front-end built with Blazor that allows users to interact with the order tracking system.

2. **Customer API**
   - Provides endpoints to manage customer details.
   - Allows retrieval and storage of customer information.

3. **Order API**
   - Manages order details and tracking information.
   - Provides endpoints to fetch order status and details.

4. **Tracking API**
   - Combines data from Customer API and Order API.
   - Merges the information and sends it to the Blazor UI for display.

5. **CustomerBlazorAuthAPI**
   - Provides authentication functionality using JWT.
   - Features two default users: 
     - `admin` (password: `admin`)
     - `emp1` (password: `emp1`)

## Features

- **Dependency Injection**: Utilizes built-in .NET Core dependency injection for managing service lifetimes and dependencies.
- **Logging**: Implements logging throughout the application for monitoring and troubleshooting.
- **Entity Framework**: Uses Entity Framework Core for database interactions, enabling easy data manipulation and retrieval.
- **JWT Authentication**: Secure access to the APIs using JSON Web Tokens for user authentication.

## Unit Testing

- **CustomerAPI.Tests**: Comprehensive unit tests for the Customer API are included to ensure functionality and reliability.

## Getting Started

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- A suitable code editor (e.g., [Visual Studio](https://visualstudio.microsoft.com/), [Visual Studio Code](https://code.visualstudio.com/))

### Setup

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/CustomerOrderTrackingApp.git
   cd CustomerOrderTrackingApp
