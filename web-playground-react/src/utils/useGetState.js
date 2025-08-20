import { useRef, useState } from 'react';
/**
 * @template T
 * @param {T} initial
 * @returns {[T, function(T):void, function():T]}
 */
function useGetState(initial) {
	const [state, setInnerState] = useState(initial);
	const immediateStateRef = useRef(state);
	let getState =() => immediateStateRef.current;

	function setState(newState) {
		const valueToSet = typeof newState === 'function' ? newState(getState()) : newState;
		setInnerState(valueToSet);
		immediateStateRef.current = valueToSet;
	}

	return [state, setState, getState];
}

export default useGetState;