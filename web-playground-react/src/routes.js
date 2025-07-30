import Home from './pages/Home';
import About from './pages/About';
import Ai from './pages/Ai';

export const ROUTES = {
  home: { path: '/', name: 'Home', component: <Home /> },
  about: { path: '/about', name: 'About', component: <About /> },
  ai: { path: '/ai', name: 'Ai', component: <Ai /> }
};