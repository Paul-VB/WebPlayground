import React from 'react';
import './Home.css';
import abe from '../assets/abe.jpg';

function Home() {
	return (
		<>
			<div className="headshot-container">
				<img src={abe} alt="Placeholder Mugshot" className="headshot" />
			</div>
			<h1>Paul Vanden Broeck</h1>
			This is a simple React home page.
		</>
	);
}

export default Home;