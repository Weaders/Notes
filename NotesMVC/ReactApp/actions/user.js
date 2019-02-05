import RestClient from './../rest/rest-client'
import UserItem from '../models/UserItem'

import history from '../common/history-app'

let restClient = new RestClient();

export const ACTION_LOGIN_START = 'LOGIN_START';
export const ACTION_LOGIN_SUCCESS = 'LOGIN_SUCCESS';
export const ACTION_LOGIN_FAIL = 'LOGIN_FAIL';

export const ACTION_REIGSTER_START = 'REGISTER_START';
export const ACTION_REGISTER_FAIL = 'REGISTER_FAIL';

export const ACTION_GET_CURRENT_START = 'GET_CURRENT_START';
export const ACTION_GET_CURRENT_END = 'GET_CURRENT_END';

export const ACTION_LOGOUT = 'LOGOUT';

export const login = (user, pwd) => {

    return async dispatch => {

        dispatch({
            type: ACTION_LOGIN_START,
            user,
            pwd
        });

        let data = {};

        try {

            data = await restClient.post('user/login', {
                user,
                password: pwd
            });

        } catch (reqError) {

            dispatch({
                type: ACTION_LOGIN_FAIL,
                reqError
            });

            return;

        }

        let userItem = new UserItem();

        userItem.id = data.id;
        userItem.username = data.username;

        dispatch({
            type: ACTION_LOGIN_SUCCESS,
            userItem
        });

    };

}

export const register = (user, pwd) => {

    return async dispatch => {

        dispatch({
            type: ACTION_REIGSTER_START,
            user,
            pwd
        });
    
        let data = null;
    
        try {
    
            data = await restClient.post('user/register', {
                user,
                password: pwd
            });
    
        } catch (reqError) {
    
            dispatch({
                type: ACTION_REGISTER_FAIL,
                reqError
            });
    
        }
    
        let userItem = new UserItem();
    
        userItem.id = data.id;
        userItem.username = data.username;
    
        dispatch({
            type: ACTION_LOGIN_SUCCESS,
            userItem
        });

    }    

}

export const logout = () => {

    return async (dispatch) => {
        
        await restClient.post('user/logout');
        dispatch({type: ACTION_LOGOUT})

    }

}

export const getCurrent = () => {

    return async (dispatch) => {

        dispatch({
            type: ACTION_GET_CURRENT_START
        });

        let data = null;

        try {
            data = await restClient.get('user/current');
        } catch (reqError) {
            
            dispatch({
                type: ACTION_GET_CURRENT_END,
                result: false,
                error: reqError
            });

            return;

        }
        
        let userItem = new UserItem();
    
        userItem.id = data.id;
        userItem.username = data.username;

        dispatch({
            type: ACTION_LOGIN_SUCCESS,
            userItem
        });

        dispatch({
            type: ACTION_GET_CURRENT_END,
            result: true,
            error: null
        });

    }
    
}