import { useState, useEffect } from 'react';
import './AiModelSelector.scss';

const AiModelSelector = ({ instance }) => {
	return (
		<div className="ai-model-selector">
			<div>
				<label htmlFor="ai-model-select">Select AI Model</label>
			</div>
			<div className={`loading-container ${instance.isLoading ? 'loading' : ''}`}>
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
		</div>
	);
};



function useAiModelSelector(defaultModelName = 'gemma3:12b') {
	const modelListUrl = `${import.meta.env.VITE_API_URL}/ai/tags`;
	const [models, setModels] = useState([]);
	const [isLoading, setIsLoading] = useState(true);
	const [selectedModel, setSelectedModel] = useState('');

	async function fetchModels() {
		try {
			const response = await fetch(modelListUrl, {
				method: 'get',
			});
			if (!response.ok) {
				throw new Error(`Failed to load models: ${response.status}`);
			}
			let data = (await response.json()).models;
			setModels(data);
			let defaultModel = (data.find(model => model.name === defaultModelName) || data[0]).model;
			setSelectedModel(defaultModel);
		} catch (error) {
			console.error('Error fetching models:', error);
		} finally {
			setIsLoading(false);
		}
	}

	useEffect(() => {
		fetchModels();
	}, []);

	return { models, selectedModel, setSelectedModel, isLoading };
}


export default AiModelSelector;
export { useAiModelSelector };
