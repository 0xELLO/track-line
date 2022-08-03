import React, { useState } from 'react'
import "../../styles/SubListStyle.css"

const AddNewSubList = (props: {addNewButtonClicked: (e: React.MouseEvent, title: string) => void}) => {

  const [title, setTitle] = useState("");

  const handleChange = (event: EventTarget & HTMLInputElement) => {
      setTitle(event.value)
  }


return (
  <form>
      <label htmlFor="title">New Sub list: </label>
      <input type="text" value={title} onChange={(e) => handleChange(e.target)} id="title"/>
      <button onClick={(e) => props.addNewButtonClicked(e, title)}>Add new</button>
  </form>
  
)
}

export default AddNewSubList