type WithDefaults<T> = {
  [K in keyof T]-?: Exclude<T[K], undefined>;
};

export function withDefaults<T extends object>(
  value: T,
  defaults: WithDefaults<T>,
): WithDefaults<T> {
  return {
    ...defaults,
    ...value,
  };
}
