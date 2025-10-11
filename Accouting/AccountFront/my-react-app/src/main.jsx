import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import {createBrowserRouter, RouterProvider, Link} from "react-router-dom";

const router = createBrowserRouter([
  {
    path: "/",
    element: 
    <>
      <Link to="/App">Go to App Page.</Link>
    </>
  },
  {
    path: "/App",
    element: 
    <>
      <Link to="/">Back.</Link>
      <App/>
    </>
  }
]);
createRoot(document.getElementById("root")).render(<RouterProvider router={router}/>);
