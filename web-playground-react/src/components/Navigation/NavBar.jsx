import './NavBar.css';
import NavBarButton from './NavBarButton';
import { useNavigate } from 'react-router-dom';

function Navbar() {
  const navigate = useNavigate();

  return (
    <div className="navbar">
      <NavBarButton label="Home" onClick={() => navigate('/')} /> 
      <NavBarButton label="About" onClick={() => navigate('/about')} />
    </div>
  );
}

export default Navbar;