import Home from 'src/pages/Home/Home';
import About from 'src/pages/About';
import Ai from 'src/pages/Ai/Ai';

export const ROUTES = {
  home: { path: '/', name: 'Home', component: <Home /> },
  about: { path: '/about', name: 'About', component: <About /> },
  ai: { path: '/ai', name: 'Ai', component: <Ai /> }
};