# Technical Test Project

This project is a technical test consisting of a client-side application and a server-side API.

## Project Structure

```
project/
├── client/   # Frontend application
├── core/     # Services layer
├── entities/ # Database model layer
├── api/      # Backend API
└── test/     # Unit tests for core

```

## Prerequisites

- Node.js (version 14 or higher)
- .NET SDK (version 5.0 or higher)
- npm

## Setup Instructions

### Client Setup

1. Navigate to the client directory:

   ```bash
   cd client
   ```

2. Install dependencies:

   ```bash
   npm install
   ```

3. Start the development server:

   ```bash
   npm run dev
   ```

   The client application will be available at `http://localhost:3000`.

### API Setup

1. Navigate to the API directory:

   ```bash
   cd api
   ```

2. Restore .NET dependencies:

   ```bash
   dotnet restore
   ```

3. Run the API:

   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:44374`.

## Usage

- Use the client application to interact with the data.
- The client communicates with the API to perform CRUD operations.

## Testing

- Ensure both the client and API are running.
- Use the client interface to test the functionality.

## License

This project is licensed under the MIT License.
