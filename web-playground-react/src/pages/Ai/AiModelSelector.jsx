import { useState, useEffect } from 'react';
import useGetState from 'src/utils/useGetState';

const AiModelSelector = ({ instance }) => {
	return (
		<div>
			<label htmlFor="ai-model-select">Select AI Model:</label>
			<select
				id="ai-model-select"
				value={instance.selectedModel.model}
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



function useAiModelSelector(defaultModelName = 'gemma3:12b') {
	const modelListUrl = `${import.meta.env.VITE_API_URL}/ai/tags`;
	const [models, setModels] = useState([]);
	const [selectedModel, setSelectedModel] = useState({});

	async function fetchModels() {
		const response = await fetch(modelListUrl, {
			method: 'get',
		});
		let data = (await response.json()).models;
		setModels(data);
		let defaultModel = data.find(model => model.name === defaultModelName) || data[0];
		console.log('Default model:', defaultModel);
		setSelectedModel(defaultModel);
	}

	useEffect(() => {
		fetchModels();
	}, []);

	return { models, selectedModel, setSelectedModel };
}


export default AiModelSelector;
export { useAiModelSelector };
