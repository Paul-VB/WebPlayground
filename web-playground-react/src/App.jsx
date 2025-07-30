import { useState } from 'react'
import { Routes, Route } from 'react-router-dom';
import { ROUTES } from './routes';
import Navbar from './components/Navigation/NavBar';
import './App.css'

function App() {
	return (
		<>
			<Navbar />
			<Routes>
				{Object.values(ROUTES).map((route) => (
					<Route path={route.path} element={route.component} />
				))}
			</Routes>
		</>
	)
}

export default App
