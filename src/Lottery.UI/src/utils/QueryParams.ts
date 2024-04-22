class QueryParams<T extends object> extends URLSearchParams {
  constructor(values: T) {
    super();
    this.processObject("", values);
  }

  private processObject(key: string, value: object) {
    if (!value) return;

    if (Array.isArray(value)) {
      return this.processArray(key, value);
    }

    for (const [k, v] of Object.entries(value)) {
      this.processProperty(this.join(key, k), v);
    }
  }

  private processArray(key: string, value: Array<unknown>) {
    for (const v of value) {
      this.processProperty(key, v);
    }
  }

  private processProperty(key: string, value: unknown) {
    if (value === null || value === undefined) return;

    switch (typeof value) {
      case "bigint":
      case "boolean":
      case "number":
      case "string":
        this.append(key, value.toString());
        break;
      case "object":
        this.processObject(key, value);
        break;
      case "function":
      case "symbol":
      case "undefined":
      default:
        break;
    }
  }

  private join(...parts: string[]): string {
    return parts.filter((p) => p).join(".");
  }
}

export default QueryParams;
