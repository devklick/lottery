import { useEffect, useRef } from "react";
import {
  SetURLSearchParams,
  createSearchParams,
  useSearchParams,
} from "react-router-dom";
import QueryParams from "../../utils/QueryParams";

export default function useMergedSearchParams<T extends object>(
  defaultInit: T
): [URLSearchParams, SetURLSearchParams] {
  const defaultSearchParams = useRef(new QueryParams(defaultInit));
  const [searchParams, setSearchParams] = useSearchParams();

  useEffect(() => {
    const params: URLSearchParams = createSearchParams(searchParams);
    let needUpdate = false;

    defaultSearchParams.current.forEach((v, k) => {
      if (!searchParams.has(k)) {
        params.append(k, v);
        needUpdate = true;
      }
    });

    if (needUpdate) setSearchParams(params);
  }, []);

  return [searchParams, setSearchParams];
}
