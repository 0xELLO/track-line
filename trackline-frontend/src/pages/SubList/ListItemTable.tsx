import React, { useEffect, useState } from 'react'
import IExtendedListItem from '../../domain/IExtendedListItem';
import { ListItemService } from '../../services/List/ListItemService'

const listItemService = new ListItemService("/list/ListItem");

const ListItemTable = (props: {subListId: string}) => {

    const [listItems, setListItems] = useState<IExtendedListItem[]>()

    const getSubLists = async () => {
        var res = await listItemService.getAllById("GetExtendedListItems", props.subListId as string) ?? []
        setListItems(res)
    }
  
    useEffect( () => {
        getSubLists().catch(console.error)
    }, [])
    
  return (
    <div>
        {listItems?.map((item, index) => {
        return <div>{item.defaultTitle}</div>
        })}
    </div>
  )
}

export default ListItemTable