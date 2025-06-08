import React from "react";
import { createRoot } from "react-dom/client";
import { 
  createBrowserRouter,
} from "react-router";
import { RouterProvider } from "react-router/dom";
import { App } from "./app";
import "./styles/styles.css";

const router = createBrowserRouter([
  {
    path: "/*",
    element: <App />,
  },
]);

const container = document.getElementById("root") as HTMLElement;

const root = createRoot(container);

root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
