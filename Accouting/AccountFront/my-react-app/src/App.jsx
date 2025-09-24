import { useState, useEffect } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'

function App() {
  const [count, setCount] = useState(0)
  const [data, setData] = useState(null)
  
  useEffect(() => {
  setData("Loading...");
  fetch("https://localhost:7244/api/WiseTest/login?id=1&password=1", {
    method: "GET",
    mode: "cors"
  })
    .then(setData("Processing...1"))
    .then(async response => {
      if (!response.ok) throw new Error("HTTP " + response.status);
      const ct = response.headers.get("content-type") || "";
      return ct.includes("application/json") ? response.json() : response.text();
    })
    .then(data => setData(data))
    .catch(error => setData("API error: " + error.message));
}, [count]);

  // useEffect(() => {
  //   setData(`Count has changed to: ${count}`);
  // }, [count]);

  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>{data}</p>
        <p>
          Edit <code>src/App.jsx</code> and save to test HMR
        </p>
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

export default App
