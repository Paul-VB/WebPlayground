import { useState } from 'react'
import { Routes, Route } from 'react-router-dom';
import { ROUTES } from './routes';
import Navbar from './components/Navigation/NavBar';
import './App.scss'

function App() {
	return (
		<div className="app-container">
			<div className="menu-container">
				<Navbar />
			</div>
			<div className='content-container'>
				<Routes>
					{Object.values(ROUTES).map((route) => (
						<Route path={route.path} element={route.component} />
					))}
				</Routes>
			</div>
		</div>
	)
}

export default App
