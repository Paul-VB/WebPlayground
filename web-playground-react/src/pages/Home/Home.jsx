import React from 'react';
import './Home.scss';
import abe from 'src/assets/abe.jpg';

function Home() {
	return (
		<div className="home page">
			<div className='page-header'>
				<h1>Paul Vanden Broeck</h1>
				<h2>Fullstack Dev</h2>
			</div>
			<div className='page-body'>
				<div className="headshot-container">
					<img src={abe} alt="Placeholder Mugshot" className="headshot" />
				</div>

				<div className="bird-container">
					<span className="bird">𓅱</span>
					<span className="bird">𓅱</span>
					<span className="bird">𓅱</span>
					<span className="bird">𓅱</span>
					<span className="bird">𓅱</span>
					<span className="bird">𓅱</span>
				</div>
			</div>

		</div>
	);
}

export default Home;