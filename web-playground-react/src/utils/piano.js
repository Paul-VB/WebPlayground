import { useState, useEffect } from 'react';
import { proxy, subscribe } from 'valtio';

export function PIANO(initial) {
    const state = useState(() => proxy(initial))[0];
    const forceUpdate = useState({})[1];
    
    useEffect(() => {
        const unsub = subscribe(state, () => forceUpdate({}));
        return unsub;
    }, []);
    
    return state; 
}