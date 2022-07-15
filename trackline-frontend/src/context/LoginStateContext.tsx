import React, {ReactNode, useContext, useState} from "react"

const LoginStateContext = React.createContext(false)
const LoginStateUpdateContext = React.createContext(() => {})

export function useLogin() {
    return useContext(LoginStateContext)
}

export function useLoginUpdate(){
    return useContext(LoginStateUpdateContext)
}

export function LoginProvider({ children } : childrenProp) {
    const [loggedIn, setLoggedIn] = useState(false)

    function toggleLoggedIn() {
        setLoggedIn(prev => !prev)
    }

    return (
        <LoginStateContext.Provider value={loggedIn}>
            <LoginStateUpdateContext.Provider value={toggleLoggedIn}>
                {children}
            </LoginStateUpdateContext.Provider>
        </LoginStateContext.Provider>
    )
}


type childrenProp = {
    children: ReactNode
}

