import React from 'react';
import peterGriffin from '../assets/Peter_Griffin.png';

function Home() {
	return (
		<>
			<h1>Home</h1>
			This is a simple React home page.
			<img src={peterGriffin} alt="Peter Griffin" />
		</>
	);
}

export default Home;