export interface IRestApiErrorResponse {
    type: string
    title: string
    status: string
    traceId: string
    errors: Map<string, string[]>
}