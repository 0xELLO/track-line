import React from 'react'
import { IRestApiErrorResponse } from '../../domain/IRestApiErrorResponse'

const ErrorView = (props: {apiErrorResponse: IRestApiErrorResponse}) => {
  return (
    <div>
        {props.apiErrorResponse.title}
        {Array.from(props.apiErrorResponse.errors).map((res, index) => {
            return res
        })}
    </div>
  )
}

export default ErrorView