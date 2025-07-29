import React from 'react';
import abe from '../assets/abe.jpg';

function Home() {
	return (
		<>
			<img src={abe} alt="Placeholder Mugshot" style={{ width: '150px', borderRadius: '50%' }} />
			<h1>Paul Vanden Broeck</h1>
			This is a simple React home page.
		</>
	);
}

export default Home;