import { useState, useEffect } from 'react';
import useGetState from 'src/utils/useGetState';

const AiModelSelector = ({ instance }) => {
	return (
		<div>
			<label htmlFor="ai-model-select">Select AI Model:</label>
			<select
				id="ai-model-select"
				value={instance.selectedModel}
				onChange={e => instance.setSelectedModel(e.target.value)}
			>
				{instance.models.map(model => (
					<option key={model.name} value={model.model}>
						{model.name}
					</option>
				))}
			</select>
		</div>
	);
};



function useAiModelSelector() {
	const modelListUrl = `${import.meta.env.VITE_API_URL}/ai/tags`;
	const [models, setModels] = useState([]);
	const [selectedModel, setSelectedModel] = useState({});

	async function fetchModels() {
		const response = await fetch(modelListUrl, {
			method: 'get',
		});
		let data = (await response.json()).models;
		setModels(data);
		if (data.length > 0) {
			setSelectedModel(data[0].model);
		}
	}

	useEffect(() => {
		fetchModels();
	}, []);

	return { models, selectedModel, setSelectedModel };
}


export default AiModelSelector;
export { useAiModelSelector };
