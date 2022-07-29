import React, { useEffect, useState } from 'react'
import { IHeadListItem } from '../../domain/IHeadListItem';
import HeadListService from '../../services/List/HeadListService';
import AddNew from './AddNew';

const headListService = new HeadListService();

const HeadList = () => {
    
    const [headListItems, setHeadListItems] = useState<IHeadListItem[]>();

    const testAdd = async (event: React.MouseEvent, title: string ) => {
        await headListService.postHeadList({
            defaultTitle: title
        })
        getHeadLists().catch(console.error)
    }

    const getHeadLists = async () => {
        var res = await headListService.getHeadLists() ?? []
        setHeadListItems(res)
    }

    useEffect( () => {
        getHeadLists().catch(console.error)
    }, [])
    
  return (
    <>
    <AddNew addNewButtonClicked = {testAdd}/>

        <div>{headListItems?.map((item, index) => {
        return  <div key={index}>{item.defaultTitle} </div> 
    })}</div>
    </>
  )
}

export default HeadList