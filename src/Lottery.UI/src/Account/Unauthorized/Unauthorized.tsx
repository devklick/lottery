// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface UnauthorizedProps {}

// eslint-disable-next-line no-empty-pattern
function Unauthorized({}: UnauthorizedProps) {
  return "Hello from Unauthorized";
}

export default Unauthorized;
