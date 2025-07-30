import './NavBar.css';
import NavBarButton from './NavBarButton';
import { ROUTES } from 'src/routes.jsx';
import { useNavigate } from 'react-router-dom';

function Navbar() {
  const navigate = useNavigate();

  return (
    <div className="navbar">
      <NavBarButton label="Home" onClick={() => navigate(ROUTES.home.path)} /> 
      <NavBarButton label="About" onClick={() => navigate(ROUTES.about.path)} />
      <NavBarButton label="Ai" onClick={() => navigate(ROUTES.ai.path)} />
    </div>
  );
}

export default Navbar;