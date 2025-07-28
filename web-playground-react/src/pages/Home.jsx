import React from 'react';
import peterGriffin from '../assets/peter_griffin.png';

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