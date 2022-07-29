export default interface IExtendedListItem {
    id? : string
    code: string
    totalLength: string
    isPublic: boolean
    isCreatedByUser: boolean
    position: number
    progress: number
    timesFinished: number
}