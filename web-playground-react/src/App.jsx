import { useState } from 'react'
import { Routes, Route } from 'react-router-dom';
import Navbar from './components/Navigation/NavBar';
import Home from './pages/Home'
import About from './pages/About'
import './App.css'

function App() {
	return (
		<>
			<Navbar />
			<Routes>
				<Route path="/" element={<Home />} />
				<Route path="/about" element={<About />} />
			</Routes>
			{}
		</>
	)
}

export default App
