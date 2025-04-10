import axios from "axios"

type Params = Record<string, unknown>
type Payload = Record<string, unknown>

//I like to use undefined through out react but having the api request only be null makes it clear to the api request doesn't need to deal with 'null'.

// Used to replace 'null' with 'undefined' in API responses so the client doesn't need to deal with 'null'.
type NullToUndefined<T> = T extends null ? undefined : T extends object ? { [K in keyof T]: NullToUndefined<T[K]> } : T
const nullToUndefined = <T extends unknown>(v: T): NullToUndefined<T> => {
    if (v === undefined || v === null) return undefined as NullToUndefined<T>
    if (typeof v !== 'object') return v as NullToUndefined<T>
    const o = (Array.isArray(v) ? [] : {}) as NullToUndefined<T>
    for (const k in v) (o as any)[k] = nullToUndefined(v[k])
    return o
}

// Used to replace 'undefined' with 'null' in API requests as the server doesn't need to deal with 'undefined'.
type UndefinedToNull<T> = T extends undefined ? null : T extends object ? { [K in keyof T]: UndefinedToNull<T[K]> } : T
const undefinedToNull = <T extends unknown>(v: T): UndefinedToNull<T> => {
    if (v === undefined || v === null) return null as UndefinedToNull<T>
    if (typeof v !== 'object') return v as UndefinedToNull<T>
    const o = (Array.isArray(v) ? [] : {}) as UndefinedToNull<T>
    for (const k in v) (o as any)[k] = nullToUndefined(v[k])
    return o
}

export const doRequest = async ({
    url, 
    method,
    params,
    payload,
} : {url: string; method: 'get' | 'post' | 'put' | 'patch' | 'delete'; params?: Params; payload?: Payload}) => {
    const {data, status} = await axios({
        url: url,
        method,
        params: params ? undefinedToNull(params) : undefined,
        data: payload ? undefinedToNull(payload) : undefined
    })

    if (status === 204) return undefined

    return nullToUndefined(data) as unknown
}

export const doMatrixRequest = async ({
    path,
    method,
    params,
    payload,
} : {path: string; method: 'get' | 'post' | 'put' | 'patch' | 'delete'; params?: Params; payload?: Payload}) => {
    const baseUrl = 'https://localhost:44374'; // Replace with an environment variable
    const url = `${baseUrl}${path}`;

    return doRequest({ url, method, params, payload });
};