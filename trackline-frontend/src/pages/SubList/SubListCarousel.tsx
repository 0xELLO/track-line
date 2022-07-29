import React from 'react'
import { ISubListItem } from '../../domain/ISubListItem'
import { SubListService } from '../../services/List/SubListService'

const SubListCarousel = async () => {

  const subListService = new SubListService()
  //let subLists = await subListService.getSubListItems();
  

  return(<div>
    
  </div>)

  return (
    <div className="row mb-5">
    <div className="col d-flex align-items-center col-auto"><button className="btn btn-primary" type="button" >&lt;</button></div>
    {/* {subLists.map((item, index) => {
        return (
            <div className="col d-flex align-items-center justify-content-center">
                <h1>{item.defaultTitle}</h1>
            </div>
        )
    })} */}
    <div className="col d-flex align-items-center col-auto"><button className="btn btn-primary" type="button">&gt;</button></div>
    </div>
  )
}

export default SubListCarousel