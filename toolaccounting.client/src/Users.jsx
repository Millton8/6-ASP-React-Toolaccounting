﻿import { useEffect, useState, } from 'react';
import { Button } from 'antd';
import { Input } from 'antd';
import errorsHandler from './ErrorsHandler';

function Users() {
    const [userFullName, setUserFullName] = useState()
    const [user, setUser] = useState()

    useEffect(() => { getUsers() }, [])
    const content=userFullName === undefined
        ? <h1>Данные загружаются</h1>
        : <div>
            <table>
                <thead>
                    <tr>
                        <th>Сотрудник</th>
                        <th>Действие</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <Input placeholder="Введите ФИО" onChange={(value) => setUser(value.target.value)} />
                        </td>
                        <td>
                            <Button onClick={addUser}>Добавить</Button>
                        </td>
                    </tr>
                    {userFullName.map(item => {
                        return <tr key={item.id}>
                            <td>{item.fullName}</td>
                            <td><Button onClick={()=>deleteUser(item.id)}>Удалить</Button></td>
                        </tr>
                    })}

                </tbody>
            </table>
            
        </div>


    async function getUsers() {
        const response = await fetch('http://localhost:5243/user');
        const data = await response.json();
        setUserFullName(data)
    }
    async function addUser() {

        const response = await fetch('http://localhost:5243/user/', {
            method: "POST",
            headers: { "Accept": "application/json", "Content-Type": "application/json" },
            body: JSON.stringify({FullName:user})
        })
        errorsHandler(response.status,getUsers())

        
    }
    async function deleteUser(id) {
        const response = await fetch(`http://localhost:5243/user/${id}`,
            { method: 'DELETE' })
        errorsHandler(response.status, getUsers())
   
    }


    return( <div>
        {content}
        
    </div>
    )
}
export default Users;