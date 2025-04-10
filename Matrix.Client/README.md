# Matrix Client

This is the client-side application for the Matrix project, built using React. The application allows users to interact with the Matrix API to manage data.

## Features

- View and manage data from the Matrix API
- User-friendly interface
- Responsive design for desktop and mobile devices

## Technologies Used

- React
- TypeScript
- CSS for styling
- Fetch API for API requests

## Getting Started

### Prerequisites

- Node.js (version 14 or higher)
- npm or yarn

### Installation

Install dependencies:

   ```bash
   npm install
   ```

### Running the Application

To start the development server, run:

```bash
npm dev
```

The application will be available at `http://localhost:3000`.

### Building for Production

To build the application for production, run:

```bash
npm run build
```

The production-ready files will be in the `build` directory.

## Project Structure

```
matrix-client/
├── public/
├── src/
│   ├── components/
│   ├── services/
│   ├── styles/
│   ├── App.tsx
│   └── index.tsx
└── package.json
```

## Configuration

The application connects to the Matrix API. You can configure the API endpoint in the `.env` file:

```
REACT_APP_API_URL=http://localhost:5000/api
```

## License

This project is licensed under the MIT License.
