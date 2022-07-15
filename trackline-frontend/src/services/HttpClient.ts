import axios from "axios";

export const httpClient = axios.create({
    baseURL: "https://localhost:7028/api/v1.0",
    headers: {
        "Content-type": "application/json"
    }
});

export default httpClient;
