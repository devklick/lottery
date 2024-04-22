import { describe, it, expect } from "vitest";

import QueryParams from "./QueryParams";

describe("QueryParams", () => {
  it.each([null, undefined])(
    "Should handle null/undefined input object",
    (input) => {
      const result = new QueryParams(input!);
      expect(result.size).toBe(0);
    }
  );

  it("Should handle flat objects", () => {
    const input = {
      a: "string",
      b: true,
      c: BigInt(0x1fffffffffffff),
      d: 123,
    };
    const result = new QueryParams(input);
    expect(result.get("a")).toBe(input.a);
    expect(result.get("b")).toBe(String(input.b));
    expect(result.get("c")).toBe(String(input.c));
    expect(result.get("d")).toBe(String(input.d));
  });

  it("Should handle null/undefined values", () => {
    const input = {
      a: "string",
      b: true,
      c: null,
      d: undefined,
    };
    const result = new QueryParams(input);
    expect(result.size).toBe(2);
    expect(result.get("a")).toBe(input.a);
    expect(result.get("b")).toBe(String(input.b));
  });

  it("Should handle nested objects", () => {
    const input = {
      a: {
        b: "string",
        c: {
          d: 123,
          e: {
            f: true,
          },
        },
      },
    };
    const result = new QueryParams(input);
    expect(result.size).toBe(3);
    expect(result.get("a.b")).toBe(input.a.b);
    expect(result.get("a.c.d")).toBe(String(input.a.c.d));
    expect(result.get("a.c.e.f")).toBe(String(input.a.c.e.f));
  });

  it("Should handle arrays", () => {
    const input = {
      a: [1, 2, 3],
      b: ["one", "two", "three"],
    };
    const result = new QueryParams(input);
    expect(result.size).toBe(input.a.length + input.b.length);
    expect(result.getAll("a")).toStrictEqual(input.a.map(String));
    expect(result.getAll("b")).toStrictEqual(input.b.map(String));
  });

  it("Should ignore functions", () => {
    const input = {
      a: () => true,
      b: "value",
    };
    const result = new QueryParams(input);
    expect(result.size).toBe(1);
    expect(result.get("b")).toBe(input.b);
  });

  it("Should ignore symbols", () => {
    const input = {
      a: Symbol("a"),
      b: "value",
    };
    const result = new QueryParams(input);
    expect(result.size).toBe(1);
    expect(result.get("b")).toBe(input.b);
  });
});
