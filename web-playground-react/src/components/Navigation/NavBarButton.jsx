import React from 'react';

const NavBarButton = ({ label, onClick }) => {
    return (
        <button onClick={onClick} className="navbar-button">
            {label}
        </button>
    );
};

export default NavBarButton;
