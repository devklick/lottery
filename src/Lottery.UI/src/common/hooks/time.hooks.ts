import { useEffect, useRef, useState } from "react";

/**
 * Wait until `waiting` becomes true or until `minTime` has elapsed, whichever comes last.
 *
 * If `waiting` is false initially, returns false immediately without waiting.
 *
 * @todo firm this up, as it's a bit nonsensical
 */
export function useWaitFor(waiting: boolean, minTime: number) {
  const [isWaiting, setIsWaiting] = useState(waiting);

  // eslint-disable-next-line react-hooks/purity
  const startedAtRef = useRef<number | null>(Date.now());

  useEffect(() => {
    if (waiting) {
      startedAtRef.current = Date.now();
      // eslint-disable-next-line react-hooks/set-state-in-effect
      setIsWaiting(true);
      return;
    }

    const startedAt = startedAtRef.current;

    if (!startedAt) {
      setIsWaiting(false);
      return;
    }

    const elapsed = Date.now() - startedAt;
    const remaining = Math.max(0, minTime - elapsed);

    const timeout = setTimeout(() => {
      setIsWaiting(false);
      startedAtRef.current = null;
    }, remaining);

    return () => clearTimeout(timeout);
  }, [waiting, minTime]);

  return isWaiting;
}
