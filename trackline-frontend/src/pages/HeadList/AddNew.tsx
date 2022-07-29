import React, { useState } from 'react'

const AddNew = (props: {addNewButtonClicked: (e: React.MouseEvent, title: string) => void}) => {

    const [title, setTitle] = useState("");

    const handleChange = (event: EventTarget & HTMLInputElement) => {
        setTitle(event.value)
    }


  return (
    <form>
        <label htmlFor="title">New Head list</label>
        <input type="text" value={title} onChange={(e) => handleChange(e.target)} id="title"/>
        <button onClick={(e) => props.addNewButtonClicked(e, title)}>adf</button>
    </form>
    
  )
}

export default AddNew